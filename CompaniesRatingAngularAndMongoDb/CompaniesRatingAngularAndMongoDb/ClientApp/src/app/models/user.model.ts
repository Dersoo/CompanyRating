export class User {
  id: string = "";
  firstName: string = "";
  lastName: string = "";
  email: string = "";
  age: number = 0;
  login: string = "";
  password: string = "";
  isAdmin: boolean = false;

  public constructor(init?: Partial<User>) {
    Object.assign(this, init);
  }
}
