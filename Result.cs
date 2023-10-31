namespace blockchain_prototype
{
    public class Result
    {
        private Blockchain blockchain;

        public Result(Blockchain blockchain)
        {
            this.blockchain = blockchain;
        }

        public Result()
        {
            blockchain = new Blockchain();
        }

        public Block GetLastBlock()
        {
            Block lastItem = blockchain.Chain.OrderBy(x => x.Index).Last();

            return lastItem;
        }
    }
}