using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation.Resources;

namespace Application
{
    public class AppLanguageManager : LanguageManager
    {
        public AppLanguageManager()
        {
            AddTranslation("en", "NotEmptyValidator", "Field should not be empty!");
            AddTranslation("en-US", "NotEmptyValidator", "Field should not be empty!");
            AddTranslation("en-GB", "NotEmptyValidator", "Field should not be empty!");
            AddTranslation("ru", "NotEmptyValidator", "Поле должно быть заполнено!");

            AddTranslation("en", "EmailValidator", "Not a valid email!");
            AddTranslation("en-US", "EmailValidator", "Not a valid email!");
            AddTranslation("en-GB", "EmailValidator", "Not a valid email!");
            AddTranslation("ru", "EmailValidator", "Некорректный email!");

            AddTranslation("en", "EqualValidator", "Fields should match!");
            AddTranslation("en-US", "EqualValidator", "Fields should match!");
            AddTranslation("en-GB", "EqualValidator", "Fields should match!");
            AddTranslation("ru", "EqualValidator", "Поля должны совпадать!");

        }
    }
}
