using System;
using System.Collections.Generic;
using System.Linq;

namespace MapReduce.Messages
{
    public class TotalResult
    {
        public Dictionary<string,int> Result { get; set; }

        public TotalResult()
        {
            Result = new Dictionary<string, int>();
        }

        public override string ToString()
        {
            return string.Format("[TotalResult: {0}]", 
                String.Join(", ", 
                    Result.Keys.Select(l => String.Format("{0}:{1}", l, Result[l]))
                )
            );
        }
    }
}
