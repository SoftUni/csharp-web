using System;
using System.Collections.Generic;
using System.Text;

namespace SIS.MvcFramework
{
    using Validation;

    public class ControllerState : IControllerState
    {
        public ModelStateDictionary ModelState { get; set; }

        public ControllerState()
        {
            this.Reset();
        }

        public void Reset()
        {
            this.ModelState = new ModelStateDictionary();
        }

        public void Initialize(Controller controller)
        {
            this.ModelState = controller.ModelState;
        }

        public void SetState(Controller controller)
        {
            controller.ModelState = this.ModelState;
        }
    }
}
