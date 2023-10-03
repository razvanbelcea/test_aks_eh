namespace eathappy.order.domain.Article
{
    public class ArticleEnums
    {
        public enum ArticleStatus
        {
            Valid,
            Voided
        }

        public enum ReasonCode
        {
            None,
            RawMaterialDeficit,
            TechnicalDifficulties,
            QualityControl,
            OtherCause
        }
    }
}
