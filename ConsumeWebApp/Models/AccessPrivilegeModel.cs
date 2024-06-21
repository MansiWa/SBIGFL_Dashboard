namespace PrintSoftWeb.Models
{
    public class AccessPrivilegeModel
    {
        public string? a_roleid { get; set; }
        public string? a_menuid { get; set; }
        public string? m_menuname { get; set; }
        public string? a_isactive { get; set; }
        public string? a_addaccess { get; set; }
        public string? a_editaccess { get; set; }
        public string? a_deleteaccess { get; set; }
        public string? a_viewaccess { get; set; }
        public string? a_workflow { get; set; }
        public bool A_AddAccessChecked => a_addaccess == "1";
        public bool A_EditAccessChecked => a_editaccess == "1";
        public bool A_DeleteAccessChecked => a_deleteaccess == "1";
        public bool A_ViewAccessChecked => a_viewaccess == "1";
        public bool A_WorkflowChecked => a_workflow == "1";
    }
}
