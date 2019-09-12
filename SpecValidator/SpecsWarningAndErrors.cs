using System.Collections.Generic;
using System.Linq;

namespace SpecValidator.SpecFlowPlugin
{
    public class SpecsWarningAndErrors
    {
        public bool HavingErrors => Errors.Any();
        public bool HavingWarnings => Warnings.Any();
        public List<MessageDetails> Errors { get; set; }
        public List<MessageDetails> Warnings { get; set; }
        public SpecsWarningAndErrors()
        {
            Errors = new List<MessageDetails>();
            Warnings = new List<MessageDetails>();
        }
    }
}