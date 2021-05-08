using System;

namespace Metozis.TeTwo.Internal.Generation.World.Processes
{
    public enum GenerationProcessType
    {
        Preprocess,
        Postprocess
    }
    
    public interface IGenerationProcess
    {
        Type DataPieceType { get; }
        string Logs { get; }
        GenerationProcessType Type { get; }
        void Generate(ref WorldGenerationMeta result, GenerationDataPiece piece);
        bool Complete { get; }
    }
}