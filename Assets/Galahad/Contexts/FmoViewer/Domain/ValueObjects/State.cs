namespace Galahad.Contexts.FmoViewer.Domain.ValueObjects
{
    public class State
    {
        public State()
        {
            this.ResidueCut = false;
        }

        public State Cut()
        {
            this.ResidueCut = true;
            return this;
        }
        public bool ResidueCut { get; set; }
    }
}