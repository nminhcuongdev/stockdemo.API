using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StockDemo.API.Data;
using StockDemo.API.Models.DTO.User;
using StockDemo.API.Repositories.UserRepository;
using StockDemo.API.Models.Domain;
using StockDemo.API.Models;
using StockDemo.API.Helpers;

namespace StockDemo.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository userRepository;
        private readonly IMapper mapper;

        public UsersController(IUserRepository userRepository, IMapper mapper)
        {
            this.userRepository = userRepository;
            this.mapper = mapper;
        }

        // GET: api/users
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await userRepository.GetAllAsync();
            var userDto = mapper.Map<List<UserDto>>(users);

            return Ok(ApiResponse<List<UserDto>>.SuccessResult(userDto, "Lấy danh sách user thành công"));
        }

        // GET: api/users/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var user = await userRepository.GetByIdAsync(id);

            if (user == null)
            {
                return NotFound(ApiResponse<UserDto>.ErrorResult("Không tìm thấy user"));
            }

            var userDto = mapper.Map<UserDto>(user);
            return Ok(ApiResponse<UserDto>.SuccessResult(userDto, "Lấy thông tin user thành công"));
        }

        // POST: api/users/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ApiResponse<LoginResponseDto>.ErrorResult(
                    "Dữ liệu không hợp lệ",
                    ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList()
                ));
            }

            // Tìm user theo username
            var user = await userRepository.GetByUsernameAsync(loginDto.Username);

            if (user == null)
            {
                return Unauthorized(ApiResponse<LoginResponseDto>.ErrorResult("Tên đăng nhập hoặc mật khẩu không đúng"));
            }

            // Kiểm tra user có active không
            if (!user.IsActive)
            {
                return Unauthorized(ApiResponse<LoginResponseDto>.ErrorResult("Tài khoản đã bị vô hiệu hóa"));
            }

            // Verify password
            bool isPasswordValid = PasswordHelper.VerifyPassword(loginDto.Password, user.PasswordHash);

            if (!isPasswordValid)
            {
                return Unauthorized(ApiResponse<LoginResponseDto>.ErrorResult("Tên đăng nhập hoặc mật khẩu không đúng"));
            }

            // Update last login date
            await userRepository.UpdateLastLoginAsync(user.UserId);

            // Tạo response
            var loginResponse = new LoginResponseDto
            {
                UserId = user.UserId,
                Username = user.Username,
                FullName = user.FullName,
                Role = user.Role,
                Token = null, // Không sử dụng JWT
                ExpiresAt = DateTime.Now.AddHours(8) // Session 8 giờ
            };

            return Ok(ApiResponse<LoginResponseDto>.SuccessResult(loginResponse, "Đăng nhập thành công"));
        }

        // POST: api/users
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateUserDto createUserDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ApiResponse<UserDto>.ErrorResult(
                    "Dữ liệu không hợp lệ",
                    ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList()
                ));
            }

            // Kiểm tra username đã tồn tại chưa
            if (await userRepository.IsUsernameExistsAsync(createUserDto.Username))
            {
                return BadRequest(ApiResponse<UserDto>.ErrorResult("Tên đăng nhập đã tồn tại"));
            }

            // Map DTO sang Entity
            var user = mapper.Map<User>(createUserDto);

            // Hash password
            user.PasswordHash = PasswordHelper.HashPassword(createUserDto.Password);
            user.CreatedDate = DateTime.Now;
            user.IsActive = true;

            // Lưu vào database
            var createdUser = await userRepository.AddAsync(user);
            var userDto = mapper.Map<UserDto>(createdUser);

            return CreatedAtAction(
                nameof(GetById),
                new { id = createdUser.UserId },
                ApiResponse<UserDto>.SuccessResult(userDto, "Tạo user thành công")
            );
        }

        // PUT: api/users/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateUserDto updateUserDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ApiResponse<UserDto>.ErrorResult(
                    "Dữ liệu không hợp lệ",
                    ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList()
                ));
            }

            var user = await userRepository.GetByIdAsync(id);

            if (user == null)
            {
                return NotFound(ApiResponse<UserDto>.ErrorResult("Không tìm thấy user"));
            }

            // Cập nhật thông tin
            if (!string.IsNullOrEmpty(updateUserDto.FullName))
                user.FullName = updateUserDto.FullName;

            if (!string.IsNullOrEmpty(updateUserDto.Role))
                user.Role = updateUserDto.Role;

            if (updateUserDto.IsActive.HasValue)
                user.IsActive = updateUserDto.IsActive.Value;

            var updatedUser = await userRepository.UpdateAsync(user);
            var userDto = mapper.Map<UserDto>(updatedUser);

            return Ok(ApiResponse<UserDto>.SuccessResult(userDto, "Cập nhật user thành công"));
        }

        // POST: api/users/{id}/change-password
        [HttpPost("{id}/change-password")]
        public async Task<IActionResult> ChangePassword([FromRoute] int id, [FromBody] ChangePasswordDto changePasswordDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ApiResponse<object>.ErrorResult(
                    "Dữ liệu không hợp lệ",
                    ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList()
                ));
            }

            var user = await userRepository.GetByIdAsync(id);

            if (user == null)
            {
                return NotFound(ApiResponse<object>.ErrorResult("Không tìm thấy user"));
            }

            // Verify old password
            bool isOldPasswordValid = PasswordHelper.VerifyPassword(changePasswordDto.OldPassword, user.PasswordHash);

            if (!isOldPasswordValid)
            {
                return BadRequest(ApiResponse<object>.ErrorResult("Mật khẩu cũ không đúng"));
            }

            // Hash new password
            user.PasswordHash = PasswordHelper.HashPassword(changePasswordDto.NewPassword);

            await userRepository.UpdateAsync(user);

            return Ok(ApiResponse<object>.SuccessResult(null, "Đổi mật khẩu thành công"));
        }

        // DELETE: api/users/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var user = await userRepository.GetByIdAsync(id);

            if (user == null)
            {
                return NotFound(ApiResponse<object>.ErrorResult("Không tìm thấy user"));
            }

            // Soft delete: chỉ set IsActive = false
            user.IsActive = false;
            await userRepository.DeleteAsync(id);

            return Ok(ApiResponse<object>.SuccessResult(null, "Xóa user thành công"));
        }

        // GET: api/users/active
        [HttpGet("active")]
        public async Task<IActionResult> GetActiveUsers()
        {
            var users = await userRepository.GetActiveUsersAsync();
            var userDto = mapper.Map<List<UserDto>>(users);

            return Ok(ApiResponse<List<UserDto>>.SuccessResult(userDto, "Lấy danh sách user active thành công"));
        }

        // GET: api/users/role/{role}
        [HttpGet("role/{role}")]
        public async Task<IActionResult> GetUsersByRole([FromRoute] string role)
        {
            var users = await userRepository.GetUsersByRoleAsync(role);
            var userDto = mapper.Map<List<UserDto>>(users);

            return Ok(ApiResponse<List<UserDto>>.SuccessResult(userDto, $"Lấy danh sách user role {role} thành công"));
        }
    }
}