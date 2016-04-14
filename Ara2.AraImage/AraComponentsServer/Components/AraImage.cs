// Copyright (c) 2010-2016, Rafael Leonel Pontani. All rights reserved.
// For licensing, see LICENSE.md or http://www.araframework.com.br/license
// This file is part of AraFramework project details visit http://www.arafrework.com.br
// AraFramework - Rafael Leonel Pontani, 2016-4-14
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Ara2.Dev;

namespace Ara2.Components
{
    [Serializable]
    [AraDevComponent(vConteiner:false,vResizable:false)]
    public class AraImage : AraComponentVisualAnchor, IAraDev
    {

        public AraImage(IAraObject ConteinerFather)
            : this(AraObjectClienteServer.Create(ConteinerFather, "img", new Dictionary<string, string> { { "src", "?araignore=1" } }), ConteinerFather)
        {
            this.MinWidth = 10;
            this.MinHeight = 10;
            this.Width = 100;
            this.Height = 100;
        }

        public AraImage(string vNameObject, IAraObject vConteinerFather)
            : base(vNameObject, vConteinerFather, "AraImage")
        {
            Click = new AraComponentEvent<EventHandler>(this, "Click");
            IsVisible = new AraComponentEvent<EventHandler>(this, "IsVisible");
            this.EventInternal += AraImage_EventInternal;
        }

        public override void LoadJS()
        {
            Tick vTick = Tick.GetTick();
            vTick.Session.AddJs("Ara2/Components/AraImage/AraImage.js");
        }

        public void AraImage_EventInternal(String vFunction)
        {
            switch (vFunction.ToUpper())
            {
                case "CLICK":
                    Click.InvokeEvent(this, new EventArgs());
                    break;
                case "ISVISIBLE":
                    IsVisible.InvokeEvent(this, new EventArgs());
                    break;
            }
        }

        #region Eventos
        [AraDevEvent]
        public AraComponentEvent<EventHandler> Click;
        [AraDevEvent]
        public AraComponentEvent<EventHandler> IsVisible;
        #endregion

        private string _Src = "";
        [AraDevProperty("")]
        public string Src
        {
            set
            {
                _Src = value;
                Tick vTick = Tick.GetTick();
                this.TickScriptCall();
                vTick.Script.Send(" vObj.SetSrc('" + AraTools.StringToStringJS(_Src) + "'); \n");
            }
            get { return _Src; }
        }

        public void RemoveInterface()
        {
            TickScriptCall();
            Tick.GetTick().Script.Send(" vObj.RemoveInterface(); \n");
        }
        
        #region Ara2Dev
        private string _Name = "";
        [AraDevProperty("")]
        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }

        private AraEvent<DStartEditPropertys> _StartEditPropertys =null;
        public AraEvent<DStartEditPropertys> StartEditPropertys 
        {
            get
            {
                if (_StartEditPropertys == null)
                {
                    _StartEditPropertys = new AraEvent<DStartEditPropertys>();
                    this.Click += this_ClickEdit;
                }

                return _StartEditPropertys;
            }
            set
            {
                _StartEditPropertys = value;
            }
        }
        private void this_ClickEdit(object sender, EventArgs e)
        {
            if (_StartEditPropertys.InvokeEvent != null)
                _StartEditPropertys.InvokeEvent(this);
        }

        private AraEvent<DStartEditPropertys> _ChangeProperty = new AraEvent<DStartEditPropertys>();
        public AraEvent<DStartEditPropertys> ChangeProperty 
        {
            get
            {
                return _ChangeProperty;
            }
            set
            {
                _ChangeProperty = value;
            }
        }

        #endregion
    }
}
