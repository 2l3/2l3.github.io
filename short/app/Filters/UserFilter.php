<?php

namespace App\Filters;

use EloquentFilter\ModelFilter;
use Illuminate\Database\Eloquent\Builder;

class UserFilter extends ModelFilter
{
    /**
     * Related Models that have ModelFilters as well as the method on the ModelFilter
     * As [relationMethod => [input_key1, input_key2]].
     *
     * @var array
     */
    public $relations = [];

    /**
     * We filter by name, surname or email.
     *
     * @param  string  $search
     * @return UserFilter
     */
    public function search($search)
    {
        return $this->where(function ($query) use ($search) {
            /** @var Builder $query */
            return $query->where('name', 'LIKE', '%'.$search.'%')
                ->orWhere('surname', 'LIKE', '%'.$search.'%')
                ->orWhere('email', 'LIKE', '%'.$search.'%');
        });
    }
}
