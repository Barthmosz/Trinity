using System.Collections.Generic;

namespace Trinity.API.ViewModels
{
    public class ResultViewModel<T>
    {
        public ResultViewModel(T data)
        {
            this.Data = data;
        }

        public ResultViewModel(List<string> errors)
        {
            this.Errors = errors;
        }

        public ResultViewModel(string error)
        {
            this.Errors.Add(error);
        }

        public T? Data { get; private set; }
        public List<string> Errors { get; private set; } = new();
    }
}
