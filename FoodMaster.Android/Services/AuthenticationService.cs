using Xamarin.Forms;
using FoodMaster.Droid.Services;
using FoodMaster.Services;
using System.Threading.Tasks;
using Firebase.Auth;
using Android.Gms.Tasks;
using System;
using FoodMaster.Models;

[assembly: Dependency(typeof(AuthenticationService))]
namespace FoodMaster.Droid.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        IAnalyticsService _analyticsService;

        public AuthenticationService()
        {
            _analyticsService = DependencyService.Get<IAnalyticsService>();
        }

        public User GetUserAsync()
        {
            var user = FirebaseAuth.Instance.CurrentUser;
            if (user != null)
            {
                return new User()
                {
                    Id = user?.Uid,
                    Email = user?.Email,
                    Names = user?.DisplayName,
                    PhoneNumber = user?.PhoneNumber,
                    PhotoUrl = user?.PhotoUrl?.ToString()
                };
            }

            return new User();
        }

        public bool IsSignIn()
        {
            var user = FirebaseAuth.Instance.CurrentUser;
            return user != null;
        }

        public async Task<string> LoginWithEmailPassword(string email, string password)
        {
            try
            {
                var user = await FirebaseAuth.Instance.SignInWithEmailAndPasswordAsync(email, password);
                var token = await user.User.GetIdToken(false).ToAwaitableTask();
                
                return token.ToString();
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.Print(e.Message);
                _analyticsService.Report(e);
                return string.Empty;
            }
        }

        public async Task<string> RegisterWithEmailPassword(string name, string email, string password)
        {
            try
            {
                var builder = new UserProfileChangeRequest.Builder().SetDisplayName(name).Build();
                
                var user = await FirebaseAuth.Instance.CreateUserWithEmailAndPasswordAsync(email, password);
                var token = await user.User.GetIdToken(false).ToAwaitableTask();
                await user.User.UpdateProfileAsync(builder).ConfigureAwait(false);
                
                return token.ToString();
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.Print(e.Message);
                _analyticsService.Report(e);
                return string.Empty;
            }
        }

        public async System.Threading.Tasks.Task UpdateName(string names)
        {
            var builder = new UserProfileChangeRequest.Builder().SetDisplayName(names).Build();
            await FirebaseAuth.Instance.CurrentUser.UpdateProfileAsync(builder).ConfigureAwait(false);
        }



        public bool SignOut()
        {
            try
            {
                FirebaseAuth.Instance.SignOut();
                return true;
            }
            catch (Exception e)
            {
                _analyticsService.Report(e);
                return false;
            }
        }
    }

    public class TaskCompleteListener : Java.Lang.Object, IOnCompleteListener
    {
        private readonly TaskCompletionSource<Java.Lang.Object> taskCompletionSource;

        public TaskCompleteListener(TaskCompletionSource<Java.Lang.Object> tcs)
        {
            this.taskCompletionSource = tcs;
        }

        public void OnComplete(Android.Gms.Tasks.Task task)
        {
            if (task.IsCanceled)
            {
                this.taskCompletionSource.SetCanceled();
            }
            else if (task.IsSuccessful)
            {
                this.taskCompletionSource.SetResult(task.Result);
            }
            else
            {
                this.taskCompletionSource.SetException(task.Exception);
            }
        }

    }

    public static class Extension
    {
        public static Task<Java.Lang.Object> ToAwaitableTask(this Android.Gms.Tasks.Task task)
        {
            var taskCompletionSource = new TaskCompletionSource<Java.Lang.Object>();
            var taskCompleteListener = new TaskCompleteListener(taskCompletionSource);
            task.AddOnCompleteListener(taskCompleteListener);

            return taskCompletionSource.Task;
        }
    }
    
}
