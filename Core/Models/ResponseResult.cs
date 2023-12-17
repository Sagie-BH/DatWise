using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class ResponseResultObj
    {
        private string _message = "";
        private StringBuilder? _stringBuilder;
        public string Message
        {
            get => _stringBuilder != null ? _stringBuilder.ToString() : _message;
            set => _message = value;
        }

        public StringBuilder? StringBuilder
        {
            get => _stringBuilder;
            set => _stringBuilder = value;
        }
        public bool IsSuccess { get; set; }
        public object? Result { get; set; } = null; 
        public OperationOutcome? customOutcome { get; set; } = null;
        public Uri? Uri { get; set; } = null;
    }
}
