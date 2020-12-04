<?php

namespace App\Models;

use App\Transformers\RoleTransformer;
use Eloquent;
use EloquentFilter\Filterable;
use Flugg\Responder\Contracts\Transformable;
use Illuminate\Database\Eloquent\Builder;
use Illuminate\Database\Eloquent\Collection;
use Illuminate\Database\Eloquent\Model;
use Illuminate\Database\Eloquent\Relations\HasMany;
use Illuminate\Support\Carbon;
use Route;

/**
 * App\Models\Role
 *
 * @property int $id
 * @property string $name
 * @property int $type
 * @property string|null $permissions
 * @property Carbon|null $created_at
 * @property Carbon|null $updated_at
 * @property-read Collection|User[] $users
 * @property-read int|null $users_count
 * @method static Builder|Role filter($input = [], $filter = null)
 * @method static Builder|Role newModelQuery()
 * @method static Builder|Role newQuery()
 * @method static Builder|Role paginateFilter($perPage = null, $columns = [], $pageName = 'page', $page = null)
 * @method static Builder|Role query()
 * @method static Builder|Role simplePaginateFilter($perPage = null, $columns = [], $pageName = 'page', $page = null)
 * @method static Builder|Role whereBeginsWith($column, $value, $boolean = 'and')
 * @method static Builder|Role whereCreatedAt($value)
 * @method static Builder|Role whereEndsWith($column, $value, $boolean = 'and')
 * @method static Builder|Role whereId($value)
 * @method static Builder|Role whereLike($column, $value, $boolean = 'and')
 * @method static Builder|Role whereName($value)
 * @method static Builder|Role wherePermissions($value)
 * @method static Builder|Role whereType($value)
 * @method static Builder|Role whereUpdatedAt($value)
 * @mixin Eloquent
 */
class Role extends Model implements Transformable
{
    use Filterable;

    /**
     * @inheritDoc
     */
    public function transformer()
    {
        return RoleTransformer::class;
    }

    /**
     * Get the users for the user role
     *
     * @return HasMany
     */
    public function users()
    {
        return $this->hasMany('App\Models\User');
    }

    /**
     * Method that checks if a route is allowed in the selected role.
     *
     * @param  string  $route
     * @return bool
     */
    public function checkPermission($route)
    {
        // If user is Admin return access to all
        if ($this->id == 1) {
            return true;
        } else {
            // Check if assigned to permission
            return in_array($route, json_decode($this->permissions));
        }
    }

    /**
     * Method to obtain the permissions of the role.
     *
     * @return array
     */
    public function getPermissions()
    {
        $controllers = [];
        $permissions = json_decode($this->permissions);
        foreach (Route::getRoutes()->getIterator() as $route) {
            if (strpos($route->uri, 'api/dashboard') !== false) {
                $path = explode('@', str_replace($route->action['namespace'].'\\', '', $route->action['controller']))[0];
                $controllers[$path] = $this->id === 1 ? true : in_array($path, $permissions);
            }
        }

        return $controllers;
    }
}
