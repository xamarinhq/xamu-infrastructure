using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace XamarinUniversity.Infrastructure
{
    /// <summary>
    /// Optional interface for ViewModels which supports
    /// initialization when being used with a Navigation
    /// </summary>
    public interface IViewModelNavigationInit
    {
        /// <summary>
        /// Method called to initialize a ViewModel
        /// </summary>
        /// <returns>Task (might be completed)</returns>
        Task IntializeAsync(object stateParameter);
    }
}
