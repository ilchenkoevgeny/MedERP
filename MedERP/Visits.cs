//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MedERP
{
    using System;
    using System.Collections.Generic;
    
    public partial class Visits
    {
        public int VisitID { get; set; }
        public int ClientID { get; set; }
        public int UserID { get; set; }
        public int SessionID { get; set; }
        public int TypeID { get; set; }
        public string Diagnosis { get; set; }
        public System.DateTime VisitDateTime { get; set; }
        public bool Removed { get; set; }
        public int RemovedByUser { get; set; }
    
        public virtual Clients Clients { get; set; }
        public virtual UserSessions UserSessions { get; set; }
        public virtual VisitType VisitType { get; set; }
    }
}
