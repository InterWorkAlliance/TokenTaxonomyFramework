export interface MenuSubOption {
  key: string;
  label: string;
}
export interface MenuOption {
  key: string;
  label: string;
  leftIcon: string;
  children: MenuSubOption[];
}

const options: MenuOption[] = [
  {
    key: 'tokenTemplates',
    label: 'Token Templates',
    leftIcon: 'ion-podium',
    children: [
      {
        key: 'formulas',
        label: 'Formulas',
      },
      {
        key: 'definitions',
        label: 'Definitions',
      },
    ],
  },
];
export default options;
