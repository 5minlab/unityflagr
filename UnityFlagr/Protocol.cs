using System.Collections.Generic;

namespace UnityFlagr
{
    public class Entity
    {
        public string entityID = null;
        public string entityType = null;
        public Dictionary<string, object> entityContext = null;
    }

    public class EvalContext
    {
        public string entityID = null;
        public string entityType = null;
        public Dictionary<string, object> entityContext = null;
        public bool enableDebug = false;
        public int? flagID = null;
        public string flagKey = null;
    }

    public class EvalResult
    {
        public int flagID = 0;
        public string flagKey = null;
        public int flagSnapshotID = 0;
        public int segmentID = 0;
        public int variantID = 0;
        public string variantKey = null;
        public Dictionary<string, object> variantAttachment = null;
        public EvalContext evalContext = null;
        public string timestamp = null;
        public EvalDebugLog evalDebugLog = null;
    }

    public class BatchEvalContext
    {
        public List<Entity> entities = null;
        public bool enableDebug = false;
        public List<int> flagIDs = null;
        public List<string> flagKeys = null;
    }

    public class BatchEvalResult
    {
        public List<EvalResult> evaluationResults = null;
    }

    public class SegmentDebugLog
    {
        public int segmentId = 0;
        public string msg = null;
    }

    public class EvalDebugLog
    {
        public List<SegmentDebugLog> segmentDebugLogs = null;
        public string msg = null;
    }
}