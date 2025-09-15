public class Portal : M_Object
{
    UIPART part = null;    

    public override void Interaction(Chracter chracter)
    {
        base.Interaction(chracter);

        part = Canvas_Holder.instance.GetUIPART("PORTAL");
        part.Open();
        part.GetComponent<PORTAL>().Init(this);
    }
}
