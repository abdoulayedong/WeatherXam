using System;
using System.Collections.Generic;
using System.Text;

namespace MeteoXamarinForms.Services.Toast
{
    public interface IToastService
    {
        void ShortToast(string message);
        void LongToast(string message);
    }
}
