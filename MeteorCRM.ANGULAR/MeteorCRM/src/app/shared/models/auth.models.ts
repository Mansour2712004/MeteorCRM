export interface LoginRequest {
    email: string;
    password: string;
}

export interface RegisterRequest {
    firstName: string;
    lastName: string;
    email: string;
    password: string;
    confirmPassword: string;
}

export interface AuthResponse {
    token: string;
    refreshToken: string;
    expiresAt: string;
    firstName: string;
    lastName: string;
    email: string;
    role: string;
}
