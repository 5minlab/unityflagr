using System.Collections.Generic;

namespace UnityFlagr
{
    class Entity
    {
        public string entityID;
        public string entityType;
        public Dictionary<string, object> entityContext;
    }

    class EvalContext
    {
        public string entityID;
        public string entityType;
        public Dictionary<string, object> entityContext;
        public bool enableDebug;
        public int? flagID;
        public string flagKey;
    }

    class EvalResult
    {
        public int flagID;
        public string flagKey;
        public int flagSnapshotID;
        public int segmentID;
        public int variantID;
        public string variantKey;
        public Dictionary<string, object> variantAttachment;
        public EvalContext evalContext;
        public string timestamp;
    }

    class BatchEvalContext
    {
        public List<Entity> entities;
        public bool enableDebug;
        public List<int> flagIDs;
        public List<string> flagKeys;
    }

    class BatchEvalResult
    {
        public List<EvalResult> evaluationResults;
    }
}