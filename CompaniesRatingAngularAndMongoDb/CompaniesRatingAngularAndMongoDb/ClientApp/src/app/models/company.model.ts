type Location = {
  city: string;
  country: string;
};

export class Company {
  id: string = "";
  name: string = "";
  rating: number = 0;
  location!: Location;
  description: string = "";

  public constructor(init?: Partial<Company>) {
    Object.assign(this, init);
  }
}
