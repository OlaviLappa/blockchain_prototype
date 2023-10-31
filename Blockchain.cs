using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using blockchain_prototype.Entities;
using static System.Reflection.Metadata.BlobBuilder;

namespace blockchain_prototype
{
    public class Blockchain
    {
        public DataFile dataFile = new DataFile();
        public List<Block> Chain { get; set; }

        public Blockchain() => InitializeBlockchain();

        public void InitializeBlockchain()
        {
            dataFile.GetChain();

            if (dataFile.GetChain().Count == 0)
            {
                Block first = new Block(DateTime.Now, "Genesis Block", "", null);

                Chain = new List<Block>
                {
                    first
                };

                dataFile.Save(Chain[0]);
            }

            else
            {
                Chain = dataFile.GetChain().OrderBy(q => q.Index).ToList();
            }
        }

        public Block AddBlock(Block newBlock, out string message)
        {
            if(CheckAllBlocks(Chain))
            {
                newBlock.Index = Chain.Count;
                newBlock.PreviousHash = Chain[Chain.Count - 1].Hash;
                newBlock.Hash = newBlock.CalculateHash();
                
                Chain.Add(newBlock);
                dataFile.Save(newBlock);

                message = "Транзакция успешно проведена. Блок сформирован.";
            }

            else
            {
                message = "Транзакция отклонена.";
                //throw new Exception();
            }

            return newBlock;
        }

        private bool CheckAllBlocks(List<Block> blocks)
        {
            if (blocks == null || blocks.Count == 0)
            {
                return false;
            }

            if(blocks.Count == 1 && blocks[0].PreviousHash == "")
            {
                return true;
            }

            bool status = false;
            var sortedBlockList = blocks.OrderBy(q => q.Index).ToList();

            string hash = null;
            int index = 0;

            foreach (Block block in sortedBlockList)
            {
                if(string.IsNullOrEmpty(block.PreviousHash))
                {
                    hash = block.Hash;

                    while(index < sortedBlockList.Count - 1)
                    {
                        if(hash == sortedBlockList[index + 1].PreviousHash)
                        {
                            index++;
                            hash = sortedBlockList[index].Hash;

                            if(sortedBlockList.Count == index)
                            {
                                status = true;
                                break;
                            }
                        }

                        else
                        {
                            return false;
                        }
                    }
                }

                else
                {
                    if(sortedBlockList.Count -1 == index)
                    {
                        return true;
                    }

                    else
                    {
                        continue;
                    }
                }
            }

            return status;
        }

        public void CheckBlock(Block block)
        {

        }
    }
}