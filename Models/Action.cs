namespace StarMarines.Models
{
    public class Action {
        public int from;
        public int to;
        public int unitsCount;

        public Action()
        {
        }

        public Action(int From, int To, int Count)
        {
            from = From;
            to = To;
            unitsCount = Count;
        }
    }
}