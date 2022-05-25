using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using web_C.Models;

namespace web_C.Controllers
{
	[Route("/api/[controller]")]

	
	public class UsersController : Controller
	{
		private static List<User> users = new List<User>(new[]
			{
				new User() { Guid = System.Guid.NewGuid(), Login = "admin", Password = "rootroot", Name = "Anastasiya",
				Gender = 0, Birthday = new DateTime(2000, 6, 8), Admin = true, CreatedOn = DateTime.Today,
					CreatedBy = "admin", RevokedOn = default, RevokedBy = "", ModifiedBy = "", ModifiedOn = default},
				new User() { Guid = System.Guid.NewGuid(), Login = "user1", Password = "password1", Name = "Vlad",
				Gender = 1, Birthday = new DateTime(2001, 2, 4), Admin = false, CreatedOn = DateTime.Today,
					CreatedBy = "user1", RevokedOn = new DateTime(2000, 6, 8), RevokedBy = "admin", ModifiedBy = "", ModifiedOn = default},
				new User() { Guid = System.Guid.NewGuid(), Login = "user2", Password = "password2", Name = "Igor",
				Gender = 2, Birthday = new DateTime(1999, 6, 23), Admin = false, CreatedOn = new DateTime(2000, 6, 8),
					CreatedBy = "admin", RevokedOn = default, RevokedBy = "", ModifiedBy = "user2", ModifiedOn = DateTime.Today},
			});

		private bool IsUserAdmin(String Login, String Password)
		{
			foreach (User u in users)
			{
				if (Login == u.Login && Password == u.Password)
				{
					if (u.Admin == true)
						return true;
				}
			}
			return false;
		}

		private bool IsUserActive(String Login, String Password)
		{
			foreach (User u in users)
			{
				if (Login == u.Login && Password == u.Password)
				{
					if (u.RevokedOn == default)
						return true;
				}
			}
			return false;
		}

		private bool IsNewLoginUniqueAndAllowed(string NewLogin)
		{
			String reg = @"^[A-Za-z0-9]+$";
			bool res = Regex.Match(NewLogin, reg).Success;
			if (res == false)
				return false;
			foreach (User u in users)
			{
				if (u.Login == NewLogin)
					return false;
			}
			return true;
		}
		private bool IsNewPasswordAllowed(string NewPassword)
		{
			String reg = @"^[A-Za-z0-9]+$";
			bool res = Regex.Match(NewPassword, reg).Success;
			return res;
		}

		private bool IsNameAllowed(string Name)
		{
			String reg = @"^[A-Za-zА-Яа-я]+$";
			bool res = Regex.Match(Name, reg).Success;
			return res;
		}


		[HttpPost("Create")]
		public IActionResult Create(string LoginNewUser, string PasswordNewUser, string NameNewUser, int GenderNewUser, DateTime BirthDayNewUser)
		{
				if (IsNewLoginUniqueAndAllowed(LoginNewUser) != true)
				{
					var modelState1 = new ModelStateDictionary();
					modelState1.AddModelError("Error", "Such Login is not allowed");
					return BadRequest(modelState1);
				}
				if (IsNewPasswordAllowed(PasswordNewUser) == false)
				{
					var modelState0 = new ModelStateDictionary();
					modelState0.AddModelError("Error", "Such Password is not allowed");
					return BadRequest(modelState0);
				}
				if (IsNameAllowed(NameNewUser) == false)
				{
					var modelState0 = new ModelStateDictionary();
					modelState0.AddModelError("Error", "Such Name is not allowed");
					return BadRequest(modelState0);
				}
				User new_user = new User();
				new_user.Guid = new Guid();
				new_user.Login = LoginNewUser;
				new_user.Password = PasswordNewUser;
				new_user.Name = NameNewUser;
				new_user.Birthday = BirthDayNewUser;
				new_user.Gender = GenderNewUser;
				new_user.Admin = false;
				new_user.CreatedOn = DateTime.Today;
				new_user.CreatedBy = LoginNewUser;
				new_user.ModifiedOn = default;
				new_user.ModifiedBy = "";
				new_user.RevokedOn = default;
				new_user.RevokedBy = "";
				users.Add(new_user);
				return Ok(new_user);
		}


		[HttpPost("CreateByAdmin")]
		public IActionResult CreateByAdmin(string Login, string Password, string LoginNewUser, string PasswordNewUser, string NameNewUser, int GenderNewUser, DateTime BirthDayNewUser, bool IsAdmin)
		{
			if (IsUserAdmin(Login, Password) == true)
			{
				if (IsNewLoginUniqueAndAllowed(LoginNewUser) != true)
				{
					var modelState1 = new ModelStateDictionary();
					modelState1.AddModelError("Error", "Such Login is not allowed");
					return BadRequest(modelState1);
				}
				if (IsNewPasswordAllowed(PasswordNewUser) == false)
				{
					var modelState0 = new ModelStateDictionary();
					modelState0.AddModelError("Error", "Such Password is not allowed");
					return BadRequest(modelState0);
				}
				if (IsNameAllowed(NameNewUser) == false)
				{
					var modelState0 = new ModelStateDictionary();
					modelState0.AddModelError("Error", "Such Name is not allowed");
					return BadRequest(modelState0);
				}
				User new_user = new User();
				new_user.Guid = System.Guid.NewGuid(); ;
				new_user.Login = LoginNewUser;
				new_user.Password = PasswordNewUser;
				new_user.Name = NameNewUser;
				new_user.Birthday = BirthDayNewUser;
				new_user.Gender = GenderNewUser;
				new_user.Admin = IsAdmin;
				new_user.CreatedOn = DateTime.Today;
				new_user.CreatedBy = Login;
				new_user.ModifiedOn = default;
				new_user.ModifiedBy = "";
				new_user.RevokedOn = default;
				new_user.RevokedBy = "";
				users.Add(new_user);
				return Ok(new_user);
			}
			else
			{
				var modelState = new ModelStateDictionary();
				modelState.AddModelError("Error", "You are not allowed to perform this action");
				return BadRequest(modelState);
			}
		}

		[HttpPost("Update")]
		public IActionResult Update(string Login, string Password, string LoginNeededUser, string NewName, int NewGender, DateTime NewBirthDay)
		{
			if (IsUserAdmin(Login, Password) == true || (Login == LoginNeededUser && IsUserActive(Login, Password) == true))
			{
				foreach (User u in users)
				{
					if (LoginNeededUser == u.Login)
					{
						u.Name = NewName;
						u.Gender = NewGender;
						u.Birthday = NewBirthDay;
						u.ModifiedOn = DateTime.Today;
						u.ModifiedBy = Login;
						return Ok(u);
					}
				}
				var modelState1 = new ModelStateDictionary();
				modelState1.AddModelError("Error", "There is no user with such 'LoginNeededUser'");
				return BadRequest(modelState1);
			}
			var modelState = new ModelStateDictionary();
			modelState.AddModelError("Error", "You are not allowed to perform this action");
			return BadRequest(modelState);
		}

		[HttpPost("Update_password")]
		public IActionResult Update_password(string Login, string Password, string LoginNeededUser, string NewPassword)
		{
			if (IsNewPasswordAllowed(NewPassword) == false)
			{
				var modelState0 = new ModelStateDictionary();
				modelState0.AddModelError("Error", "Such NewPassword is not allowed");
				return BadRequest(modelState0);
			}

			if (IsUserAdmin(Login, Password) == true || (Login == LoginNeededUser && IsUserActive(Login, Password) == true))
			{
				foreach (User u in users)
				{
					if (LoginNeededUser == u.Login)
					{
						u.Password = NewPassword;
						u.ModifiedOn = DateTime.Today;
						u.ModifiedBy = Login;
						return Ok(u);
					}
				}
				var modelState1 = new ModelStateDictionary();
				modelState1.AddModelError("Error", "There is no user with such 'LoginNeededUser'");
				return BadRequest(modelState1);
			}
			var modelState = new ModelStateDictionary();
			modelState.AddModelError("Error", "You are not allowed to perform this action");
			return BadRequest(modelState);
		}

		[HttpPost("Update_login")]
		public IActionResult Update_login(string Login, string Password, string OldLoginNeededUser, string NewLogin)
		{
			if (IsNewLoginUniqueAndAllowed(NewLogin) == false)
			{
				var modelState0 = new ModelStateDictionary();
				modelState0.AddModelError("Error", "Such NewLogin is not allowed");
				return BadRequest(modelState0);
			}

			if (IsUserAdmin(Login, Password) == true || (Login == OldLoginNeededUser && IsUserActive(Login, Password) == true))
			{				
				foreach (User u in users)
				{
					if (OldLoginNeededUser == u.Login)
					{
						u.Login = NewLogin;
						u.ModifiedOn = DateTime.Today;
						u.ModifiedBy = Login;
						return Ok(u);
					}
				}
				var modelState1 = new ModelStateDictionary();
				modelState1.AddModelError("Error", "There is no user with such 'LoginNeededUser'");
				return BadRequest(modelState1);
			}
			var modelState = new ModelStateDictionary();
			modelState.AddModelError("Error", "You are not allowed to perform this action");
			return BadRequest(modelState);
		}

		[HttpGet("Read")]
		public IEnumerable<User> Read(string Login, string Password) {
			if (IsUserAdmin(Login, Password) == true)
				{
				List<User> res = new List<User>();
				foreach (User u in users)
				{
					if (u.RevokedOn == default)
						res.Add(u);
				}
				res.Sort(delegate (User usr1, User usr2) { return usr1.CreatedOn.CompareTo(usr2.CreatedOn); });
				return res;
			}
			else
				return null;
		}

		[HttpGet("Read_by_login")]
		public IEnumerable<UserByLogin> Read_by_login(string Login, string Password, string Login_needed_user)
		{
			if (IsUserAdmin(Login, Password) == true)
			{
				List<UserByLogin> res = new List<UserByLogin>();
				UserByLogin res_user = new UserByLogin();
				foreach (User u in users)
				{
					if (u.Login == Login_needed_user)
					{
						res_user.Name = u.Name;
						res_user.Gender = u.Gender;
						if (u.RevokedOn == default)
							res_user.Status = "active";
						else
							res_user.Status = "inactive";
					}
				}
				res.Add(res_user);
				return res;
			}
			else
				return null;
		}

		[HttpGet("Read_by_login_and_password")]
		public IEnumerable<User> Read_by_login_and_password(string Login, string Password, string Login_needed_user, string Password_needed_user)
		{
			List<User> res = new List<User>();
			foreach (User u in users)
			{
				if (u.Login == Login && u.Password == Password && Login == Login_needed_user && Password == Password_needed_user && u.RevokedOn == default)
				{
					res.Add(u);
				}
			}
			return res;
		}

		[HttpGet("Read_by_age")]
		public IEnumerable<User> Read_by_age(string Login, string Password, int Age)
		{
			if (IsUserAdmin(Login, Password) == true)
			{
					List<User> res = new List<User>();
				foreach (User u in users)
				{
					int user_age = CalculateAge(u.Birthday);
					if (user_age > Age)
					{
						res.Add(u);
					}
				}
				return res;
			}
			else
				return null;
		}

		private int CalculateAge(DateTime birthday)
		{
			DateTime today = DateTime.Today;
			int age = today.Year - birthday.Year;
			if (birthday.AddYears(age) > today)
			{
				age--;
			}
			return age;
		}

		[HttpPost("Hard_delete")]
		public IActionResult Hard_delete(string Login, string Password, string LoginNeededUser)
		{
			if (IsUserAdmin(Login, Password) == true)
			{
				int index = -1;
				for (int i = 0; i < users.Count; i++)
				{
					if (users[i].Login == LoginNeededUser)
						index = i;
				}
				if (index == -1)
				{
					var modelState1 = new ModelStateDictionary();
					modelState1.AddModelError("Error", "There is no user with such 'LoginNeededUser'");
					return BadRequest(modelState1);
				}
				else
				{
					users.RemoveAt(index);
					return Ok();
				}
			}
			else
			{
				var modelState = new ModelStateDictionary();
				modelState.AddModelError("Error", "You are not allowed to perform this action");
				return BadRequest(modelState);
			}
		}

		[HttpPost("Soft_delete")]
		public IActionResult Soft_delete(string Login, string Password, string LoginNeededUser)
		{
			if (IsUserAdmin(Login, Password) == true)
			{
				foreach (User u in users)
				{
					if (u.Login == LoginNeededUser)
					{
						u.RevokedBy = Login;
						u.RevokedOn = DateTime.Today;
						return Ok();
					}
				}
				var modelState1 = new ModelStateDictionary();
				modelState1.AddModelError("Error", "There is no user with such 'LoginNeededUser'");
				return BadRequest(modelState1);
			}
			else
			{
				var modelState = new ModelStateDictionary();
				modelState.AddModelError("Error", "You are not allowed to perform this action");
				return BadRequest(modelState);
			}
		}

		[HttpPost("Restore_user")]
		public IActionResult RestoreUser(string Login, string Password, string LoginNeededUser)
		{
			if (IsUserAdmin(Login, Password) == true)
			{
				foreach (User u in users)
				{
					if (u.Login == LoginNeededUser)
					{
						u.RevokedBy = "";
						u.RevokedOn = default;
						return Ok();
					}
				}
				var modelState1 = new ModelStateDictionary();
				modelState1.AddModelError("Error", "There is no user with such 'LoginNeededUser'");
				return BadRequest(modelState1);
			}
			else
			{
				var modelState = new ModelStateDictionary();
				modelState.AddModelError("Error", "You are not allowed to perform this action");
				return BadRequest(modelState);
			}
		}

	}
}
