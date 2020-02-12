using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FinalProject_NailSalon
{
    class MessageHelper
    {
        public static void ShowWarningMessage(string message, string caution)
        {
            MessageBox.Show(message, caution, MessageBoxButton.OK, MessageBoxImage.Warning);
        }
        
        public static MessageBoxResult ShowQuestionMessage(string message, string caution)
        {
            return MessageBox.Show(message, caution, MessageBoxButton.YesNo, MessageBoxImage.Question);
        }
    }
}
