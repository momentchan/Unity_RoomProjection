using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace mj.gist.projection {
    public interface IFieldUser  {
        FieldType[] GetFieldTypes();
        Dictionary<FieldType, IField> Fields { get; set; }
    }
}