export interface LoginModel {
    email: string;
    password: string;
}

export interface LoginResponse {
    id: number;
    email: string;
    role: string;
    token: string;
}