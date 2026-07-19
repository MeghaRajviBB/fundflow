import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { Auth } from '../../services/auth';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [FormsModule, RouterLink],
  templateUrl: './login.html',
  styleUrl: './login.css'
})
export class Login {
  name = '';
  role = 'Admin';

  constructor(private auth: Auth, private router: Router) {}

  signIn() {
    const name = this.name.trim();
    if (!name) return;
    this.auth.login(name, this.role);
    this.router.navigate(['/app']);
  }
}
