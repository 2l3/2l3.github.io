<?php

use App\Models\Role;
use Illuminate\Database\Seeder;

class RolesTableSeeder extends Seeder
{
    /**
     * Run the database seeds.
     *
     * @return void
     */
    public function run()
    {
        if (Role::count() == 0) {
            // Add Admin Role
            $role = new Role();
            $role->name = 'Admin';
            $role->type = 1;
            $role->permissions = '[]';
            $role->save();
            // Add User Role
            $role = new Role();
            $role->name = 'User';
            $role->type = 1;
            $role->permissions = json_encode(['LinkController']);
            $role->save();
        }
    }
}
