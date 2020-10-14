import ITemporaryStorageForHandleFields from "./model";

export default class TemporaryStorageForHandleFields implements ITemporaryStorageForHandleFields.Impl {
  private fields: ITemporaryStorageForHandleFields.Fields = {};

  setFieldValue(id:string, value: string): void {
    this.fields[id] = {
      value
    };
  }

  updateFieldValue(id:string, value: string): void {
    this.fields[id].value = value;
  }

  getFieldValue(id:string): string {
    return this.fields[id].value;
  }

  getFields(): ITemporaryStorageForHandleFields.Fields {
    return this.fields;
  }
}