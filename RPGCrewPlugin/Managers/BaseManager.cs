using Torch.API;
using RPGCrewPlugin.Data;

namespace RPGCrewPlugin.Managers
{
    public abstract class BaseManager
    {
        protected IConnectionFactory ConnectionFactory { get; }
        protected ITorchBase Torch
        {
            get { return RPGCrewPlugin.Instance.Torch; }
        }

        protected RPGCrewConfig Config
        {
            get { return RPGCrewPlugin.Instance.Config; }
        }

        public BaseManager(IConnectionFactory connectionFactory)
        {
            ConnectionFactory = connectionFactory;
        }

        public virtual void Update() { }
        public virtual void Stop() { }
        public virtual void Start() { }
        public virtual void Save() { }
    }
}
