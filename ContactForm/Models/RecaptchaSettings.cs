using System;
using System.Collections.Generic;
using System.Text;

namespace ContactForm.Models
{
    public class RecaptchaSettings
    {
        public string RecaptchaKey { get; set; }
        public string RecaptchaResponse { get; set; }
        public bool Enabled { get; set; }
    }
}
