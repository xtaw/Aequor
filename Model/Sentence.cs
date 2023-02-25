using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aequor.Model
{

    internal class Sentence
    {

        public int id;
        public string text;
        public string categoryText;
        public string source;
        public string sourceCharacter;

        public Sentence(int id, string text, string categoryText, string source, string sourceCharacter)
        {
            this.id = id;
            this.text = text;
            this.categoryText = categoryText;
            this.source = source;
            this.sourceCharacter = sourceCharacter;
        }

    }

}
