namespace DatWise
{
    public sealed class AppSettings
    {
        public string? NewImagePath { get; init; }
        public string? ZapImagesPath { get; init; }
        public string? PicSizes { get; init; }
        public string[]? PicDetailsArr { get => PicSizes?.Split(new string[] { "||" }, StringSplitOptions.RemoveEmptyEntries); }
        public string? SyteApiPath { get; set; }
        public string? SyteSignature { get; set; }
        public string? SyteAccountId { get; set; }
        public string? ClearBgKey { get; set; }
        public string? ScannerPath { get; set; }
        public int Batch { get; set; }
    }
}
