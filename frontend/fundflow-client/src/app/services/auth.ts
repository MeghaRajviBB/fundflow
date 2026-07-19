import { Injectable } from '@angular/core';

export interface User {
  name: string;
  role: string;
}

@Injectable({
  providedIn: 'root'
})
export class Auth {
  private user: User | null = null;

  login(name: string, role: string): void {
    this.user = { name, role };
  }

  logout(): void {
    this.user = null;
  }

  isLoggedIn(): boolean {
    return this.user !== null;
  }

  getUser(): User | null {
    return this.user;
  }

  getRole(): string | null {
    return this.user ? this.user.role : null;
  }
}
