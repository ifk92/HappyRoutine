import { PostsComponent } from './components/post-components/posts/posts.component';
import { RegisterComponent } from './components/register/register.component';
import { PostEditComponent } from './components/post-components/post-edit/post-edit.component';
import { PostCardComponent } from './components/post-components/post-card/post-card.component';
import { PostComponent } from './components/post-components/post/post.component';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './components/home/home.component';
import { LoginComponent } from './components/login/login.component';
import { NotFoundComponent } from './components/not-found/not-found.component';
import { AuthGuard } from './guards/auth.guard';

const routes: Routes = [
  {path: '', component: HomeComponent},
  {path: 'home', component: HomeComponent},
  {path: 'login', component: LoginComponent},
  {path: 'register', component: RegisterComponent},
  {path: 'posts', component: PostsComponent},
  {path: 'postEdit', component: PostEditComponent},
  {path: 'posts/:id', component: PostComponent},
  {path: 'dashboard/:id', component: PostEditComponent, canActivate: [AuthGuard]},
  {path: 'not-found', component: NotFoundComponent},
  {path: '**', redirectTo: '/not-found'}
];
@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
