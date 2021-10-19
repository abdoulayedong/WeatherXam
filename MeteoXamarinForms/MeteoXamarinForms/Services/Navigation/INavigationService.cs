using System.Threading.Tasks;

namespace MeteoXamarinForms.Services.Navigation
{
    public interface INavigationService
    {
        /// <summary>
        /// Navigation method to push into the Navigation Stack
        /// </summary>
        /// <typeparam name="TViewModelBase"></typeparam>
        /// <param name="navigationData"></param>
        /// <param name="setRoute"></param>
        /// <returns></returns>
        Task NavigateToAsync<TViewModelBase>(object navigationData = null, bool setRoute = false);

        /// <summary>
        /// Navigation method to pop off of the Navigation Stack
        /// </summary>
        /// <returns></returns>
        Task GoBackAsync();

        /// <summary>
        /// Navigation method to close the application
        /// </summary>
        /// <returns></returns>
        void CloseAsync();
    }
}