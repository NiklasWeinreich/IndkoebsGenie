export interface User {
    id: number;
    firstName: string;
    lastName: string;
    email: string;
    address: string;
    city: string;
    postalCode: string;
    region: string;
    role: string;
}

export function resetUser(){
    return {
    id: 0,
    firstName: "",
    lastName: "",
    email: "",
    address: "",
    city: "",
    postalCode: 0,
    region: "",
    role: ""
    }
}