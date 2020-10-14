namespace ITemporaryStorageForHandleFields {
  export interface Impl {
    getFields(): Fields;
    setFieldValue(id: string, value: string): void;
    getFieldValue(id: string): string;
    updateFieldValue(id:string, value: string): void;
  }

  export interface Field {
    value: string;
  }

  export interface Fields {
    [key: string]: Field
  }
}

export default ITemporaryStorageForHandleFields;