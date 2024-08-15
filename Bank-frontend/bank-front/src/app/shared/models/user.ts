export interface User {
    id: string;
    userName: string;
    firstName: string;
    lastName: string;
    email: string;
  }


  export interface UserRegistrationDto {
    firstName: string;
    lastName: string;
    email: string;
    userName: string;
    password: string;
  }

  export interface UserLoginDto {
    userName: string;
    password: string;
  }
  
  
  