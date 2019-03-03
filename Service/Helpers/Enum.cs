using System.ComponentModel;


namespace Services.Helpers
{
    public enum EnumFilterOp
    {
        Equals,
        GreaterThan,
        LessThan,
        GreaterThanOrEqual,
        LessThanOrEqual,
        Contains,
        StartsWith,
        EndsWith
    }
    public enum EnumDaily
    {
        Membership,
        Project,
        PersonalTrainer,
        Pos,
        Collection,
        Transfer,
        Freeze
    }

    public enum EnumPosisi
    {

        Instructor,
        Sales,
        [Description("Personal Trainer")]
        Trainer
    }

    public enum EnumStatusAccounting
    {
        Closed,
        Open,
        Post
    }

    public enum EnumAcountingStatus
    {
        Closed,
        Checked,
        Post,
        Null
    }

    public enum EnumGender
    {
        Male = 'M',
        Female = 'F'
    }


    public enum EnumStatusAction
    {
        [Description("Freeze")]
        Freeze,
        [Description("Cancel")]
        Cancel,
        [Description("Upgrade")]
        Upgrade,
        [Description("Card")]
        Card
    }

    public enum EnumPaymentType
    {
        Debit,
        Credit,
        Cash
    }

    public enum EnumModuleForm
    {
        Aktifitas,
        Office,
        Registrasi,
        Auth,
        Accounting
    }

    public enum EnumMemberCheck
    {
        [Description("In")]
        In,
        [Description("Out")]
        Out
    }

    #region member
    public enum EnumMemberState
    {
        Calon,
        Member,
        Closed
    }

    public enum EnumStatusRegister
    {
        Calon,
        Closed
    }

    public enum EnumStatusMember
    {
        [Description("CalonMember")]
        CalonMember,
        [Description("InProcessMembership")]
        InProcessMembership,
        [Description("Membership")]
        Membership,
        [Description("Freeze")]
        Freeze,
        [Description("Transfer")]
        Transfer,
        [Description("Card Printing")]
        CardPrinting,
        [Description("Personal Trainer")]
        PersonalTrainer
    }
    #endregion

    public enum EnumImageType
    {
        Ktp = 0,
        Kredit = 1,
        Debit = 2,
        Foto = 3
    }

    public enum EnumMessageType
    {
        Success,
        Information,
        Warning,
        Danger,
    }
}
