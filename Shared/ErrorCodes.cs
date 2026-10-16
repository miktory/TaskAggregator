namespace TaskAggregator.Shared;

public static class ErrorCodes
{
    public static class Domain
    {
        public const string RequestExternalIdMustBeUniqueWithinRequestSourceId = "D-0";
        public const string RequestSourceExternalIdMustBeUnique = "D-1";

    }

    public static class Application
    {
        public const string FileProcessingError = "A-0";
        public const string ChainedEntityNotFoundById = "A-1";
        public const string ExcelColumnValueIsInvalid = "A-2";
        public const string ReferencedEntityNotFoundByExternalId = "A-3";
        public const string BaseDataPersistenceError = "A-4";
        public const string InvalidOffsetPagingParametersError = "A-5";
    }
}
