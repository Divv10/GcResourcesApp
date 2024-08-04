using GCManagementApp.Windows;
using System.Windows;
using System.Windows.Controls;

namespace GCManagementApp.TemplateSelectors
{
    public class GoogleStatusTemplateSelector : DataTemplateSelector
    {
        private DataTemplate _notLoggedTemplate;
        public DataTemplate NotLoggedTemplate
        {
            get => _notLoggedTemplate;
            set => _notLoggedTemplate = value;
        }

        private DataTemplate _loggingTemplate;
        public DataTemplate LoggingTemplate
        {
            get => _loggingTemplate;
            set => _loggingTemplate = value;
        }

        private DataTemplate _loggedTemplate;
        public DataTemplate LoggedTemplate
        {
            get => _loggedTemplate;
            set => _loggedTemplate = value;
        }

        private DataTemplate _errorTemplate;
        public DataTemplate ErrorTemplate
        {
            get => _errorTemplate;
            set => _errorTemplate = value;
        }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var selectedTemplate = NotLoggedTemplate;

            if (!(item is GoogleLoginStatus status))
                return selectedTemplate;

            switch (status)
            {
                default:
                case GoogleLoginStatus.NotLogged: return NotLoggedTemplate;
                case GoogleLoginStatus.Logging: return LoggingTemplate;
                case GoogleLoginStatus.Logged: return LoggedTemplate;
                case GoogleLoginStatus.Error: return ErrorTemplate;
            }
        }
    }
}
