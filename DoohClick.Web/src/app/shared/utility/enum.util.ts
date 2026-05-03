import { OptionObj } from '../model/utility.model';

export function enumToOptions<T extends object>(enumObj: T): OptionObj<T[keyof T]>[] {
  return (Object.keys(enumObj) as (keyof T)[])
    .filter(k => isNaN(Number(k)))
    .map(
      k =>
        ({
          label: k as string,
          value: enumObj[k]
        }),
    );
}
