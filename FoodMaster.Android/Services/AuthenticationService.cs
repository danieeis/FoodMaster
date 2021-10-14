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
        public User GetUserAsync()
        {
            var user = FirebaseAuth.Instance.CurrentUser;

            return new User()
            {
                Id = user.TenantId,
                Email = user.Email,
                Names = user.DisplayName
            };

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
                return string.Empty;
            }
        }

        public async Task<string> RegisterWithEmailPassword(string email, string password)
        {
            try
            {
                var user = await FirebaseAuth.Instance.CreateUserWithEmailAndPasswordAsync(email, password);
                var token = await user.User.GetIdToken(false).ToAwaitableTask();

                return token.ToString();
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.Print(e.Message);
                return string.Empty;
            }
        }



        public bool SignOut()
        {
            try
            {
                FirebaseAuth.Instance.SignOut();
                return true;
            }
            catch (Exception)
            {
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
