export const shapeFieldValue = (newValue: string | undefined, oldValue: string) => {
  return newValue !== undefined ? newValue : oldValue;
};