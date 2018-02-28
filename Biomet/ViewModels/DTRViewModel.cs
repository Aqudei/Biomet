using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DPFP;
using DPFP.Verification;

namespace Biomet.ViewModels
{
    public class DTRViewModel : CaptureFingerViewModel
    {
        private Verification Verificator;

        protected override void Init()
        {
            base.Init();
            Verificator = new DPFP.Verification.Verification();
        }

        protected override void Process(Sample Sample)
        {
            base.Process(Sample);

        }
    }
}
