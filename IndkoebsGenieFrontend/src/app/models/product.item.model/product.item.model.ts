export interface ProductItem {
    id: number;
    name: string;
    quantity: number;
    category: number;
    notes: string;
    isCompleted: boolean;
    groceryListId: number;
}