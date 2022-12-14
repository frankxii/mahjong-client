namespace MVC.Base
{
    public class BaseModel<T> where T : BaseModel<T>, new()
    {
        private static T _instance;
        private static object locker = new();

        public static T Instance
        {
            get
            {
                // 使用锁保证线程安全，实现单例
                if (_instance is null)
                {
                    lock (locker)
                    {
                        if (_instance is null)
                            _instance = new T();
                    }
                }

                return _instance;
            }

            set => _instance = value;
        }
    }
}